# PhD Theses and Master's Dissertations Platform
is a C++ platform (integrated with **ML.NET**) for managing **PhD theses** and **Master’s dissertations**.  
It offers students, professors, and administrators a secure and interactive space to upload, review, and explore academic research.

## Features
- **Upload & Save Theses**: Students can submit their research once approved by supervisor
- **Supervisor Approval**: Professors review and validate uploads before publication
- **Personalized Dashboard**: Each user has their own feed upon login
- **Profile System**: Follow other users and track their publications
- **Commenting System**: Leave feedback on theses with 
  - ML-based moderation that automatically hides inappropriate comments  
  - Suspicious comments are sent to the admin for review
- **Advanced Search**: Locate research by **title, language, author, or keywords**
- **Secure Login**: Access restricted to university email accounts with password recovery options

## Tech Stack
- ![C++](https://img.shields.io/badge/C%2B%2B-00599C?style=for-the-badge&logo=c%2B%2B&logoColor=white) C++ – Core logic
- ![WPF](https://img.shields.io/badge/WPF-512BD4?style=for-the-badge&logo=windows&logoColor=white) WPF – Modern UI & dashboards
- ![ML.NET](https://img.shields.io/badge/ML.NET-512BD4?style=for-the-badge&logo=.net&logoColor=white) ML.NET – Comment moderation
- ![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white) MySQL – Database backend
- ![Email](https://img.shields.io/badge/Email-FD9D24?style=for-the-badge) University login & recovery

## Usage
1. Clone the repository
   ```bash
   git clone https://github.com/mmellyo/PhD-Theses-and-Master-s-Dissertations-Platform
