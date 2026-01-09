create database if not exists attendance;
use attendance;

CREATE TABLE Clients (
    ClientID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100),
    Phone VARCHAR(20)
);

CREATE TABLE Services (
    ServiceID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    DurationMinutes INT NOT NULL
);

CREATE TABLE Appointments (
    AppointmentID INT AUTO_INCREMENT PRIMARY KEY,
    ClientID INT NOT NULL,
    ServiceID INT NOT NULL,
    Date DATE NOT NULL,
    Time TIME NOT NULL,
    Notes VARCHAR(255),
    FOREIGN KEY (ClientID) REFERENCES Clients(ClientID) ON DELETE CASCADE,
    FOREIGN KEY (ServiceID) REFERENCES Services(ServiceID) ON DELETE CASCADE
);



insert into Services (Name, DurationMinutes) values
('Manicure', 60),
('Pedicure', 60),
('Chimeric Antigen Receptor T-Cell Therapy Session', 180),
('Hyperthermic Intraperitoneal Chemotherapy Infusion', 240),
('Autologous Stem Cell Rejuvenation Treatment', 210),
('Extracorporeal Photopheresis Procedure', 150),
('Proton Beam Precision Oncology Session', 120),
('Oncolytic Viral Immunotherapy Consultation', 90),
('High-Intensity Focused Ultrasound Ablation Therapy', 130),
('Gene-Edited Cytokine Modulation Therapy', 200),
('Nano-Encapsulated Drug Delivery Infusion', 110),
('Monoclonal Antibody Immune Checkpoint Blockade Session', 160);

insert into Clients (Name, Email, Phone) values
('Maritoni Zara','emailnizara@gmail.com','09123456789'),
('Ronalyn Abuyan','abuyaaaaan@gmail.com','09112233445'),
('Jemar Agraviador','jemsiii@gmail.com','09676769691');



drop database attendance;
drop table Services;
drop table Clients;
drop table Appointments;

SET SQL_SAFE_UPDATES = 0;
delete from Services;
delete from Clients;

select * from Services;
select * from Clients;
select * from Appointments;