Drop Database If Exists DB;
CREATE DATABASE DB;

Use DB;

DROP TABLE IF EXISTS `People`;

CREATE TABLE `People` (
	`Id` VARCHAR(36) NOT NULL PRIMARY KEY, 
	`FullName` VARCHAR(255),
	`Age` INT NOT NULL,
	`Height` INT NOT NULL,
	`Dob` DATETIME NULL
) ENGINE=INNODB;

-- Stored Procedures
DROP PROCEDURE IF EXISTS `SelectPeople`;
DROP PROCEDURE IF EXISTS `SelectPeopleWithParam`;
DROP PROCEDURE IF EXISTS `CountPeople`;
DROP PROCEDURE IF EXISTS `CountPeopleWithParam`;
DROP PROCEDURE IF EXISTS `UpdatePeople`;
DROP PROCEDURE IF EXISTS `UpdatePeopleWithParam`;


CREATE PROCEDURE `SelectPeople` ()
	SELECT * FROM `People`;

CREATE PROCEDURE `SelectPeopleWithParam` (age INT)
	SELECT * FROM `People` WHERE `People`.`Age` > age;

CREATE PROCEDURE `CountPeople` () 
	SELECT COUNT(*) FROM `People`;

CREATE PROCEDURE `CountPeopleWithParam` (age INT)
	SELECT COUNT(*) FROM `People` WHERE `People`.`Age` > age;

CREATE PROCEDURE `UpdatePeople` () 
	UPDATE `People ` SET ` FullName` = 'Updated'; 

CREATE PROCEDURE `UpdatePeopleWithParam` (age INT)
	UPDATE `People ` SET ` FullName` = 'Updated' WHERE `People`.`Age` > age;