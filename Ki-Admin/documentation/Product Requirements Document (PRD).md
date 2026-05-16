Product Requirements Document (PRD) – Support Hub
1. Project Overview
The goal is to implement a clean, modular system for admin/user management, authentication, and role-based access control, with the potential to expand into a full support platform.
For the initial development, the project will be hosted locally using XAMPP with Apache server and MySQL as the database. This setup allows for easy configuration and testing in a local environment. Once the application is complete and fully tested, the final step will be to migrate and deploy the project to IONOS using FileZilla.
📌 Additional Notes:
- The project adopts a modular folder structure (e.g., separating concerns across Controllers/Auth, Models, Services, etc.) for improved organization, testability, and scalability.
- Vite will be used as the frontend asset bundler for modern, faster builds and hot-reloading during development.
- MySQL will serve as the primary relational database for storing user data, support tickets, and knowledge base articles.
2. Goals & Objectives
- Provide a clean, intuitive interface for teachers to submit and track support tickets.
- Enable admins to manage tickets, users, and knowledge base content.
- Implement secure, role-based access control.
- Lay the groundwork for future expansion (e.g., chat support, analytics).
3. Key Features
Support Ticket System
- Teachers can create, view, and update tickets.
- Admins can assign, respond to, and close tickets.
- Ticket statuses: Open, In Progress, Waiting on 3rd Party, Resolved, Closed.
- Tickets support all types of attachments, limited to 5MB.
- Internal notes with an option to email each set of notes.
Knowledge Base
- Admins can create and categorize articles.
- Teachers can search and read articles.
- Articles include rich text formatting, links, images, tags, attachments, and feedback (helpful/not helpful).
User Roles & Permissions
- SuperAdmin: Full access and control.
- Admin: Manage other users and set access to certain parts of the website, but cannot delete users.
- Teachers: Can create/view their own tickets and read articles.
- Support Agents: Limited access, but can be granted additional access by Admin or SuperAdmin.
- Email notifications for ticket updates.
Authentication & Authorization
- Multi-guard authentication using Laravel’s built-in features.
- Role-based middleware for route protection.
- Secure registration, login, and logout.
- Registration available only to users with email ending in '@orchard.leics.sch.uk'.
- Email verification and password reset functionality.
Dashboard
  - Ticket statistics (open, closed, pending).
  - User activity.
  - Article engagement.
4. Technical Architecture
| Layer | Technology |
|-------|------------|
| Backend | Laravel  |
| Frontend |  (Blade + Vite) |
| Auth | Laravel Guards + Middleware |
| Structure | Modular MVC (Controllers/Auth, Models, Services, etc.) |
| Database | MySQL |
| Real-time Updates | Laravel Echo + Laravel WebSockets |
| Queues | Laravel Queues |
| Deployment | XAMPP (local) -> IONOS (production) |
5. Development Workflow
Approach:
- Step-by-step development with clear explanations.
- Each feature is broken down into:
  - What it does
  - Why it’s needed
  - How it’s implemented
Each step includes:
- Code snippets
- Laravel feature explanations
- A test plan
6. Test Plan Template (Per Feature)
| Section | Description |
|---------|-------------|
| **What Are We Testing?** | Describe the feature or component. |
| **Why Are We Testing It?** | Justify the test’s purpose. |
| **Expected Outcome** | Define what success looks like. |
| **How to Verify** | Manual steps or automated test instructions. |
7. Milestones (Suggested)
| Milestone | Description |
|----------|-------------|
| ✅ Setup | Laravel + Vite + Modular structure |
| 🎫 Ticket System | CRUD for tickets, status updates |
| 📚 Knowledge Base | Article management and search |
| 👥 User Roles | Role-based access and guards |
| 🔐 Auth | Registration, login, logout |
| 📊 Dashboard | Stats and UI polish |
| 🚀 Deployment | Push to IONOS |

