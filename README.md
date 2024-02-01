Sql query to create the tables

-- Create Classes table
CREATE TABLE Classes (
    ClassID INT PRIMARY KEY IDENTITY(1,1),
    ClassName VARCHAR(50)
);

-- Create Courses table
CREATE TABLE Courses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    CourseName VARCHAR(50)
);

-- Create Students table with foreign keys referencing Classes and Courses
CREATE TABLE Students (
    StudentID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    ClassID INT, 
    CourseID INT, 
    FOREIGN KEY (ClassID) REFERENCES Classes(ClassID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);

-- Create Grades table with foreign keys referencing Students and Courses
CREATE TABLE Grades (
    GradeID INT PRIMARY KEY IDENTITY(1,1),
    GradeName INT, 
    GradeDate DATE,
    StudentID INT, 
    CourseID INT, 
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);

-- Create Teacher table with a foreign key referencing Classes
CREATE TABLE Teachers (
    TeacherID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50),
    SchoolRole VARCHAR(50),
    ClassID INT, -- Foreign key referencing Classes
    FOREIGN KEY (ClassID) REFERENCES Classes(ClassID)
);
