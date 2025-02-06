-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 06, 2025 at 04:58 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `library_of_life`
--

-- --------------------------------------------------------

--
-- Table structure for table `admininfo`
--

CREATE TABLE `admininfo` (
  `ID` int(11) NOT NULL,
  `Admin_Username` varchar(255) NOT NULL,
  `Admin_Password` varchar(255) NOT NULL,
  `Created_At` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `admininfo`
--

INSERT INTO `admininfo` (`ID`, `Admin_Username`, `Admin_Password`, `Created_At`) VALUES
(1, '1', '1', '2025-02-03 01:04:30');

-- --------------------------------------------------------

--
-- Table structure for table `books`
--

CREATE TABLE `books` (
  `Book_Id` int(11) NOT NULL,
  `Book_Name` varchar(255) NOT NULL,
  `Book_Author` varchar(255) DEFAULT NULL,
  `Book_Genre` varchar(100) DEFAULT NULL,
  `Published_Year` year(4) DEFAULT NULL,
  `ISBN` varchar(20) DEFAULT NULL,
  `Created_At` timestamp NOT NULL DEFAULT current_timestamp(),
  `Book_Location` varchar(255) NOT NULL,
  `Book_Stocks` int(11) NOT NULL,
  `Date_Added` date NOT NULL,
  `Image_Path` varchar(255) NOT NULL,
  `Status` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `books`
--

INSERT INTO `books` (`Book_Id`, `Book_Name`, `Book_Author`, `Book_Genre`, `Published_Year`, `ISBN`, `Created_At`, `Book_Location`, `Book_Stocks`, `Date_Added`, `Image_Path`, `Status`) VALUES
(1, 'Game of Thrones2', 'Meee', 'Horror', NULL, NULL, '2025-02-03 01:33:10', '12-4A', 4, '0000-00-00', 'Resources\\120250203173957.png', 'Available');

-- --------------------------------------------------------

--
-- Table structure for table `book_history`
--

CREATE TABLE `book_history` (
  `ID` int(11) NOT NULL,
  `Book_ID` int(11) NOT NULL,
  `Change_Name` varchar(255) DEFAULT NULL,
  `Change_Author` varchar(255) DEFAULT NULL,
  `Change_Location` varchar(255) DEFAULT NULL,
  `Change_Stocks` int(11) DEFAULT NULL,
  `Change_Genre` varchar(255) DEFAULT NULL,
  `Change_Date` text NOT NULL,
  `Remarks` varchar(100) DEFAULT NULL,
  `Image_Path` varchar(255) DEFAULT NULL,
  `Initial_Name` varchar(255) DEFAULT NULL,
  `Initial_Author` varchar(255) DEFAULT NULL,
  `Initial_Location` varchar(255) DEFAULT NULL,
  `Initial_Stocks` int(11) DEFAULT NULL,
  `Initial_Genre` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `book_history`
--

INSERT INTO `book_history` (`ID`, `Book_ID`, `Change_Name`, `Change_Author`, `Change_Location`, `Change_Stocks`, `Change_Genre`, `Change_Date`, `Remarks`, `Image_Path`, `Initial_Name`, `Initial_Author`, `Initial_Location`, `Initial_Stocks`, `Initial_Genre`) VALUES
(1, 1, 'Game of Thrones', 'Meee', '12-4A', 3, 'Horror', '0000-00-00 00:00:00', 'ADD', 'Resources\\1.png', NULL, NULL, NULL, NULL, NULL),
(2, 1, 'Game of Thrones2', 'Meee', '12-4A', 3, 'Horror', '0000-00-00 00:00:00', 'EDIT', 'Resources\\120250203093319.png', 'Game of Thrones', 'Meee', '12-4A', 3, 'Horror'),
(3, 1, 'Game of Thrones2', 'Meee', '12-4A', 3, 'Horror', '02-03-2025', 'EDIT', 'Resources\\120250203173957.png', 'Game of Thrones2', 'Meee', '12-4A', 0, 'Horror');

-- --------------------------------------------------------

--
-- Table structure for table `borrowedbook`
--

CREATE TABLE `borrowedbook` (
  `ID` int(11) NOT NULL,
  `member_ID` varchar(255) NOT NULL,
  `Reference_ID` varchar(255) NOT NULL,
  `Borrowed_Date` text NOT NULL,
  `Return_Date` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `borrowedbook`
--

INSERT INTO `borrowedbook` (`ID`, `member_ID`, `Reference_ID`, `Borrowed_Date`, `Return_Date`) VALUES
(3, '202500001', '915826621', '02-03-2025 17:34:47', '02-10-2025 17:34:47');

-- --------------------------------------------------------

--
-- Table structure for table `borrowlist`
--

CREATE TABLE `borrowlist` (
  `ID` int(11) NOT NULL,
  `Reference_ID` varchar(255) NOT NULL,
  `Book_List` text NOT NULL,
  `Book_Remarks` text DEFAULT 'No remarks',
  `Borrowed_Date` text NOT NULL,
  `Return_Date` text NOT NULL,
  `Status` varchar(255) NOT NULL,
  `Initial_Status` varchar(255) NOT NULL,
  `Returned_Date` text NOT NULL,
  `Violation` text NOT NULL,
  `New_Remarks` text NOT NULL,
  `Returned_Time` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `borrowlist`
--

INSERT INTO `borrowlist` (`ID`, `Reference_ID`, `Book_List`, `Book_Remarks`, `Borrowed_Date`, `Return_Date`, `Status`, `Initial_Status`, `Returned_Date`, `Violation`, `New_Remarks`, `Returned_Time`) VALUES
(3, '915826621', '1', '', '02-03-2025', '02-10-2025', 'Returned', 'Borrowed', '02-03-2025', 'LATE', '', '17:37:07'),
(4, '915826621', '1', '', '02-03-2025', '02-10-2025', 'Returned', 'Borrowed', '02-03-2025', ' ', '', '17:40:18');

-- --------------------------------------------------------

--
-- Table structure for table `genre_list`
--

CREATE TABLE `genre_list` (
  `ID` int(11) NOT NULL,
  `Genre` varchar(255) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `genre_list`
--

INSERT INTO `genre_list` (`ID`, `Genre`, `created_at`, `updated_at`) VALUES
(2, 'Horror', '2025-02-03 01:29:50', '2025-02-03 01:29:50');

-- --------------------------------------------------------

--
-- Table structure for table `history_member`
--

CREATE TABLE `history_member` (
  `ID` int(11) NOT NULL,
  `member_ID` varchar(255) NOT NULL,
  `Initial_Name` varchar(255) NOT NULL,
  `Edited_Name` varchar(255) DEFAULT 'NONE',
  `Initial_Age` int(11) NOT NULL,
  `Edited_Age` int(11) NOT NULL,
  `Initial_Address` text NOT NULL,
  `Edited_Address` text DEFAULT 'NONE',
  `Initial_Contact` varchar(255) NOT NULL,
  `Edited_Contact` varchar(255) DEFAULT 'NONE',
  `Initial_Email` varchar(255) NOT NULL,
  `Edited_Email` varchar(255) DEFAULT 'NONE',
  `Date` text NOT NULL,
  `Remarks` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `history_member`
--

INSERT INTO `history_member` (`ID`, `member_ID`, `Initial_Name`, `Edited_Name`, `Initial_Age`, `Edited_Age`, `Initial_Address`, `Edited_Address`, `Initial_Contact`, `Edited_Contact`, `Initial_Email`, `Edited_Email`, `Date`, `Remarks`) VALUES
(1, '202500001', 'Orcullo, Mark O.', 'NONE', 20, 0, 'SB2', 'NONE', '09951025876', 'NONE', 'markjoshua@gmail.com', 'NONE', '0000-00-00', 'ADD');

-- --------------------------------------------------------

--
-- Table structure for table `location_list`
--

CREATE TABLE `location_list` (
  `ID` int(11) NOT NULL,
  `Location_Name` varchar(255) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `location_list`
--

INSERT INTO `location_list` (`ID`, `Location_Name`, `created_at`, `updated_at`) VALUES
(1, '12-4A', '2025-02-03 01:29:59', '2025-02-03 01:29:59');

-- --------------------------------------------------------

--
-- Table structure for table `members`
--

CREATE TABLE `members` (
  `ID` int(11) NOT NULL,
  `First_Name` varchar(255) NOT NULL,
  `Last_Name` varchar(255) NOT NULL,
  `MI` varchar(10) DEFAULT NULL,
  `Actual_ID` varchar(50) DEFAULT NULL,
  `Age` int(11) DEFAULT NULL CHECK (`Age` >= 0),
  `Address` text NOT NULL,
  `Contact_Number` varchar(20) NOT NULL,
  `Email_Address` varchar(255) DEFAULT NULL,
  `Registration_Year` int(11) NOT NULL,
  `Registration_Date` text NOT NULL,
  `Status` enum('Regular','Inactive','Banned') DEFAULT 'Regular',
  `Presented_ID` varchar(255) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `members`
--

INSERT INTO `members` (`ID`, `First_Name`, `Last_Name`, `MI`, `Actual_ID`, `Age`, `Address`, `Contact_Number`, `Email_Address`, `Registration_Year`, `Registration_Date`, `Status`, `Presented_ID`, `created_at`, `updated_at`) VALUES
(1, 'Mark', 'Orcullo', 'O.', '202500001', 20, 'SB2', '09951025876', 'markjoshua@gmail.com', 2025, '0000-00-00', 'Regular', 'PhilHealth ID', '2025-02-03 01:09:51', '2025-02-03 01:09:51');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `admininfo`
--
ALTER TABLE `admininfo`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `Admin_Username` (`Admin_Username`);

--
-- Indexes for table `books`
--
ALTER TABLE `books`
  ADD PRIMARY KEY (`Book_Id`),
  ADD UNIQUE KEY `ISBN` (`ISBN`);

--
-- Indexes for table `book_history`
--
ALTER TABLE `book_history`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `Book_ID` (`Book_ID`);

--
-- Indexes for table `borrowedbook`
--
ALTER TABLE `borrowedbook`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `borrowlist`
--
ALTER TABLE `borrowlist`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `genre_list`
--
ALTER TABLE `genre_list`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `Genre` (`Genre`);

--
-- Indexes for table `history_member`
--
ALTER TABLE `history_member`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `location_list`
--
ALTER TABLE `location_list`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `Location_Name` (`Location_Name`);

--
-- Indexes for table `members`
--
ALTER TABLE `members`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `Actual_ID` (`Actual_ID`),
  ADD UNIQUE KEY `Email_Address` (`Email_Address`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `admininfo`
--
ALTER TABLE `admininfo`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `books`
--
ALTER TABLE `books`
  MODIFY `Book_Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `book_history`
--
ALTER TABLE `book_history`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `borrowedbook`
--
ALTER TABLE `borrowedbook`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `borrowlist`
--
ALTER TABLE `borrowlist`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `genre_list`
--
ALTER TABLE `genre_list`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `history_member`
--
ALTER TABLE `history_member`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `location_list`
--
ALTER TABLE `location_list`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `members`
--
ALTER TABLE `members`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `book_history`
--
ALTER TABLE `book_history`
  ADD CONSTRAINT `book_history_ibfk_1` FOREIGN KEY (`Book_ID`) REFERENCES `books` (`Book_Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
