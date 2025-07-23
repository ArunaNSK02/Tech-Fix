# 🔧 TechFix – Service-Oriented Repair Management System

**TechFix** is a web-based repair management system developed for the *Service-Oriented Computing (SOC)* module using **ASP.NET Web Forms** and **SOAP-based Web Services**. The system is designed for a local computer repair shop to optimize procurement, inventory management, and service coordination between clients, suppliers, and administrators.

---

## 🎯 Project Purpose

The goal of TechFix is to eliminate inefficiencies in the traditional manual repair service and procurement processes by implementing a **Service-Oriented Architecture (SOA)**. The application allows multiple user roles to interact seamlessly via modular, service-based communication.

---

## 🚀 Features

### 🧑‍💼 Admin Dashboard
- Approve or reject user (Recipient) registrations  
- Manage all users (Donors, Recipients, Admins, Delivery Personnel)  
- View and manage donations, complaints, and blog content  
- Monitor delivery tracking and completed transactions

### 👨‍🔧 Client Application (TechFix Customers)
- Create service quotation requests with file attachments  
- Select individual suppliers or all suppliers for quotation  
- View supplier responses and itemized quotations  
- Track order statuses and manage communication

### 🏪 Supplier Application
- View and respond to quotations  
- Manage products and inventory  
- Update stock levels and pricing  
- Restrict deletion of older stock linked to past orders

---

## 🏗️ Architecture Overview

This project follows **Service-Oriented Architecture (SOA)** principles:

- 🔗 **SOAP Web Services**: Enables communication between Client and Supplier apps  
- 🧩 **Modular Applications**: Separate projects for client and supplier interfaces  
- 📁 **Shared Folder**: Contains reusable logic and files shared between applications  
- 🔐 **Role-Based Session Management**: Session storage for `user_id` and access control

---

## 🛠️ Technologies Used

- **ASP.NET Web Forms (C#)**
- **SOAP Web Services**
- **ADO.NET** for database interaction
- **SQL Server** as the database engine
- **HTML, CSS, JavaScript** for frontend
- **Bootstrap** for UI responsiveness

---

## 🗃️ Database Design Highlights

- Normalized tables: Users, Products, Quotations, Inventory, Deliveries, Complaints  
- File attachments stored as BLOBs (optional) or file paths  
- Logical deletions using `is_active` field  
- Quantity tracking and updates per supplier  
- Session tracking for user authentication

---

## 🧠 Learning Outcomes

- Designed and implemented a working **Service-Oriented system**
- Gained hands-on experience with **SOAP services** and **ASP.NET Web Forms**
- Built **scalable, modular** software with proper separation of concerns
- Practiced secure file handling, user role management, and dynamic UIs


😊!
