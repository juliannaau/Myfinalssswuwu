document.addEventListener("DOMContentLoaded", () => {
    const clientSelect = document.getElementById("clientSelect");
    const serviceSelect = document.getElementById("serviceSelect");
    const appointmentDate = document.getElementById("appointmentDate");
    const appointmentTime = document.getElementById("appointmentTime");
    const notesInput = document.getElementById("notesInput");
    const saveBtn = document.getElementById("saveAppointment");
    const appointmentBody = document.getElementById("appointmentBody");
    let editingId = null;

    function setToday() {
        const now = new Date();
        const year = now.getFullYear();
        const month = String(now.getMonth() + 1).padStart(2, "0");
        const day = String(now.getDate()).padStart(2, "0");
        appointmentDate.value = `${year}-${month}-${day}`;
        appointmentTime.value = "";
    }

    setToday();

    fetch("/api/appointments/clients")
        .then(res => res.json())
        .then(data => {
            clientSelect.innerHTML = "<option disabled selected>Select Client</option>";
            data.forEach(c => {
                const option = document.createElement("option");
                option.value = c.clientID;
                option.textContent = c.name;
                clientSelect.appendChild(option);
            });
        });

    fetch("/api/appointments/services")
        .then(res => res.json())
        .then(data => {
            serviceSelect.innerHTML = "<option disabled selected>Select Service</option>";
            data.forEach(s => {
                const option = document.createElement("option");
                option.value = s.serviceID;
                option.textContent = `${s.name} (${s.durationMinutes} min)`;
                serviceSelect.appendChild(option);
            });
        });

    function loadAppointments() {
        fetch("/api/appointments/all")
            .then(res => res.json())
            .then(data => {
                appointmentBody.innerHTML = "";
                data.forEach(a => {
                    const row = document.createElement("tr");
                    row.innerHTML = `
                        <td>${a.client?.name || ""}</td>
                        <td>${a.service?.name || ""}</td>
                        <td>${new Date(a.date).toLocaleDateString()}</td>
                        <td>${a.time}</td>
                        <td>${a.notes || ""}</td>
                        <td>
                            <button onclick='editAppointment(${JSON.stringify(a)})'>Edit</button>
                            <button style="background:#b71c1c" onclick="deleteAppointment(${a.appointmentID})">Delete</button>
                        </td>
                    `;
                    appointmentBody.appendChild(row);
                });
            });
    }

    saveBtn.addEventListener("click", () => {
        if (!clientSelect.value || !serviceSelect.value || !appointmentDate.value || !appointmentTime.value) {
            alert("Please fill in all required fields (Client, Service, Date, Time).");
            return;
        }
        const payload = {
            clientID: parseInt(clientSelect.value),
            serviceID: parseInt(serviceSelect.value),
            date: appointmentDate.value,
            time: appointmentTime.value,
            notes: notesInput.value
        };
        let url = "/api/appointments";
        let method = "POST";
        let body = JSON.stringify([payload]);
        if (editingId) {
            url = `/api/appointments/${editingId}`;
            method = "PUT";
            body = JSON.stringify(payload);
        }
        fetch(url, {
            method,
            headers: { "Content-Type": "application/json" },
            body
        })
            .then(res => {
                if (!res.ok) throw new Error();
                clientSelect.value = "";
                serviceSelect.value = "";
                setToday();
                notesInput.value = "";
                saveBtn.textContent = "Add Appointment";
                editingId = null;
                loadAppointments();
            });
    });

    window.editAppointment = function (a) {
        clientSelect.value = a.clientID;
        serviceSelect.value = a.serviceID;
        const d = new Date(a.date);
        const year = d.getFullYear();
        const month = String(d.getMonth() + 1).padStart(2, "0");
        const day = String(d.getDate()).padStart(2, "0");
        appointmentDate.value = `${year}-${month}-${day}`;
        appointmentTime.value = a.time;
        notesInput.value = a.notes || "";
        saveBtn.textContent = "Update Appointment";
        editingId = a.appointmentID;
    };

    window.deleteAppointment = function (id) {
        if (!confirm("Are you sure you want to delete this appointment?")) return;
        fetch(`/api/appointments/${id}`, { method: "DELETE" })
            .then(() => loadAppointments());
    };

    loadAppointments();
});
