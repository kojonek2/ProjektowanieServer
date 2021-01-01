DROP TABLE IF EXISTS standard_routes_data
DROP TABLE IF EXISTS custom_routes_data
DROP TABLE IF EXISTS routes
DROP TABLE IF EXISTS leader_qualifications
DROP TABLE IF EXISTS mountain_group
DROP TABLE IF EXISTS leader_data
DROP TABLE IF EXISTS turists_data
DROP TABLE IF EXISTS users
GO

CREATE TABLE users (
	id INT IDENTITY PRIMARY KEY,
	login VARCHAR(40) UNIQUE NOT NULL,
	password CHAR(48) NOT NULL,
	email VARCHAR(50) NOT NULL,
	isAdmin BIT NOT NULL DEFAULT 0,
	joinDate DATE NOT NULL DEFAULT GETDATE()
)

INSERT INTO users(isadmin, login, email, password) VALUES (1, 'admin', 'admin@gmail.com', '/gFdKiSaODPcyZNwBipWbNJAStwhrnfCLhFYMzBon+zoOP1E') --haslo: admin
INSERT INTO users(login, email, password) VALUES ('turysta', 'turysta.zmeczony@gmail.com', 'Ptqk1oarroB94b/hHV9PkI/XlHHsEsduU5IAeoJ2QuxCFdDy') --haslo: turysta123
INSERT INTO users(login, email, password) VALUES ('przodownik', 'przod.najlepszy@gmail.com', 'VeZsvu0K/rFI8P08z5ITR/mO4WfPlu2wovQMUrhhZqj1tgVV') --haslo: przodownik
INSERT INTO users(login, email, password) VALUES ('przodownik2', 'przod.najlepszy@gmail.com', 'VeZsvu0K/rFI8P08z5ITR/mO4WfPlu2wovQMUrhhZqj1tgVV') --haslo: przodownik
INSERT INTO users(login, email, password) VALUES ('franek', 'franek.test@gmail.com', 'vCp46N/R5p9p9mkomAcJLa407E7MjsSu/X3c6kq+44Jqw8hV') --haslo: qwerty

CREATE TABLE turists_data (
	userId INT PRIMARY KEY FOREIGN KEY REFERENCES users(id),
	firstName VARCHAR(50),
	sureName VARCHAR(50)
)

INSERT INTO turists_data VALUES((SELECT id FROM users WHERE login = 'turysta'), 'Adam', 'Ma³ysz')
INSERT INTO turists_data VALUES((SELECT id FROM users WHERE login = 'przodownik'), 'Marian', 'Skalisty')
INSERT INTO turists_data VALUES((SELECT id FROM users WHERE login = 'przodownik2'), 'Maria', 'Nowak')

CREATE TABLE leader_data (
	userId INT PRIMARY KEY FOREIGN KEY REFERENCES turists_data(userId),
	nominationDate DATE NOT NULL DEFAULT GETDATE(),
	resignationDate DATE,
	CHECK (nominationDate <= resignationDate)
)

INSERT INTO leader_data(userId) VALUES((SELECT id FROM users WHERE login = 'przodownik'))
INSERT INTO leader_data(userId) VALUES((SELECT id FROM users WHERE login = 'przodownik2'))

CREATE TABLE mountain_group (
	id INT IDENTITY PRIMARY KEY,
	name VARCHAR(50) UNIQUE NOT NULL,
	abbreviation VARCHAR(15) NOT NULL
)

INSERT INTO mountain_group(name, abbreviation) VALUES ('Tatry wysokie', 'T.01')
INSERT INTO mountain_group(name, abbreviation) VALUES ('Beskidy Œl¹sk', 'BZ.01')
INSERT INTO mountain_group(name, abbreviation) VALUES ('Beskid Niski', 'BW.01')

CREATE TABLE leader_qualifications (
	leaderId INT FOREIGN KEY REFERENCES leader_data(userId),
	mountainGroupId int FOREIGN KEY REFERENCES mountain_group(id),
	PRIMARY KEY(leaderId, mountainGroupId)
)

INSERT INTO leader_qualifications VALUES ((SELECT id FROM users WHERE login = 'przodownik'), (SELECT id FROM mountain_group WHERE abbreviation = 'T.01'))
INSERT INTO leader_qualifications VALUES ((SELECT id FROM users WHERE login = 'przodownik2'), (SELECT id FROM mountain_group WHERE abbreviation = 'BW.01'))

CREATE TABLE routes (
	id INT IDENTITY PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	length DECIMAL NOT NULL,
	sumOfClimbs INT NOT NULL,
	mountainGroupId INT FOREIGN KEY REFERENCES mountain_group(id)
)

INSERT INTO routes(name, length, sumOfClimbs, mountainGroupId) VALUES ('Trasa Stulecia', 2.5, 356, (SELECT id FROM mountain_group WHERE name = 'Beskidy Œl¹sk'))
INSERT INTO routes(name, length, sumOfClimbs, mountainGroupId) VALUES ('Trasa jubileuszowa', 1.23, 500, (SELECT id FROM mountain_group WHERE name = 'Beskidy Œl¹sk'))
INSERT INTO routes(name, length, sumOfClimbs, mountainGroupId) VALUES ('W ko³o slê¿y', 10, 100, (SELECT id FROM mountain_group WHERE name = 'Beskid Niski'))

CREATE TABLE custom_routes_data (
	routeId INT PRIMARY KEY FOREIGN KEY REFERENCES routes(id),
	description VARCHAR(300) NOT NULL
)

INSERT INTO custom_routes_data VALUES ((SELECT id FROM routes WHERE name = 'W ko³o slê¿y'), 'Obejœcie œlê¿y w ko³o zgodnie z wskazówkami zegara patrz¹c na mapê skierowan¹ gór¹ do pó³nocy')

CREATE TABLE standard_routes_data (
	routeId INT PRIMARY KEY FOREIGN KEY REFERENCES routes(id),
	openingDate DATETIME NOT NULL,
	closingDate DATETIME,
	walkingTime INT NOT NULL,
	difficulty VARCHAR(50) NOT NULL,
	CHECK(openingDate <= closingDate)
)

INSERT INTO standard_routes_data VALUES ((SELECT id FROM routes WHERE name = 'Trasa Stulecia'), '2000-01-01', NULL, 75, 'Easy')
INSERT INTO standard_routes_data VALUES ((SELECT id FROM routes WHERE name = 'Trasa jubileuszowa'), '2010-01-01', NULL, 45, 'Moderate')