# **University Management System**

### **Overview**
The **University Management System** is a comprehensive platform built to streamline university operations by automating administrative, academic, and student activities. It leverages modern technologies such as facial recognition for login and attendance and supports role-based functionalities for students, professors, and affairs staff.

The system is developed using **.NET Core MVC** to provide a robust, maintainable, and scalable web application.

---

### **Key Features**
- **Facial Recognition Integration**:
  - Login and attendance via face recognition for students and professors.
  - Traditional login (username and password) is also supported.
  
- **Role-Based Access**:
  - **Students**:
    - View personal information, enrolled courses, and available lectures.
  - **Professors**:
    - Add lectures, create assignments, and manage attendance.
  - **Affairs Staff**:
    - Manage users, courses, and professors with CRUD operations.
  
- **Responsive Design**:
  - Optimized for desktop and mobile devices.

- **User-Friendly Interface**:
  - Easy navigation with clean and modern UI design.

---

### **Technologies Used**

#### **Frontend**
- **HTML5** & **CSS3**: To structure and style the web pages.
- **Bootstrap**: For creating responsive and visually appealing layouts.
- **JavaScript**: For handling dynamic and interactive elements.
- **Face Recognition Integration**: Built with JavaScript libraries to enable seamless facial detection and authentication.

#### **Backend**
- **.NET Core MVC**:
  - Used to build the application’s structure, logic, and views.
  - Ensures smooth interaction between frontend and backend.
- **Entity Framework Core**: To manage database operations and handle ORM (Object Relational Mapping).

#### **Database**
- **SQL Server**: Used for storing and managing data, including user information, courses, lectures, and attendance records.

#### **Version Control**
- **Git** & **GitHub**: For source code management and collaboration.

---

### **Screenshots**

#### Homepage
![Homepage](Screenshots/website-home-page.jpg)

#### Login
- **Username & Password**  
  ![Login with Username](Screenshots/log-in-with-username-pass.jpg)  
- **Facial Recognition**  
  ![Login with FaceID](Screenshots/log-in-faceid.jpg)  

#### Student Dashboard
- **Personal Information**  
  ![Student Info](Screenshots/stu-info.jpg)  
- **Courses**  
  ![Student Courses](Screenshots/stu-courses.jpg)  
- **Lectures**  
  ![Student Lectures](Screenshots/stu-lec.jpg)  

#### Professor Dashboard
- **Professor Overview**  
  ![Professor UI](Screenshots/doc-ui.jpg)  
- **Take Attendance Using FaceID**  
  ![Attendance by FaceID](Screenshots/doc-take-attendance-by-faceid.jpg)  
- **Courses Management**  
  ![Professor Courses](Screenshots/doc-courses.jpg)  
- **Add Lecture**  
  ![Add Lecture](Screenshots/doc-add-lec.jpg)  
- **Add Assignment**  
  ![Add Assignment](Screenshots/doc-add-assignment.jpg)  

#### Affairs Dashboard
- **Affairs Overview**  
  ![Affairs UI](Screenshots/affairs-ui.jpg)  
- **Add Professor**  
  ![Add Professor](Screenshots/affairs-add-doc.jpg)  
- **Search for Professor's Courses**  
  ![Search Professor Courses](Screenshots/affairs-search-doc-courses.jpg)  
- **Search for Students**  
  ![Search Students](Screenshots/affairs-search-stu.jpg)  
- **Add New Course**  
  ![Add Course](Screenshots/affairs-add-course.jpg)  

---

### **How the System Works**

1. **Students**:
   - Students log in using either their username/password or facial recognition.
   - Once logged in, they can view their personal information, enrolled courses, and upcoming lectures.

2. **Professors**:
   - Professors log in with their credentials or facial recognition.
   - They can create new lectures, add assignments, and manage attendance for their courses.
   - Attendance is taken using a facial recognition system to ensure accuracy.

3. **Affairs Staff**:
   - Affairs staff have complete control over the system and can:
     - Add, delete, or modify student, professor, and course data.
     - Search for specific records to retrieve detailed information.

---

### **Setup Instructions**

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/yourusername/university-management-system.git
   cd university-management-system
