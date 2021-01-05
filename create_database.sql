DROP TABLE IF EXISTS badge_application_tours
DROP TABLE IF EXISTS badge_applications
DROP TABLE IF EXISTS entries
DROP TABLE IF EXISTS tours
DROP TABLE IF EXISTS badge_ranks
DROP TABLE IF EXISTS badges
DROP TABLE IF EXISTS standard_routes_data
DROP TABLE IF EXISTS custom_routes_data
DROP TABLE IF EXISTS routes
DROP TABLE IF EXISTS route_points
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

CREATE TABLE route_points (
	id INT IDENTITY PRIMARY KEY,
	name VARCHAR(50) UNIQUE NOT NULL,
	description VARCHAR(500),
	cordinates CHAR(27) NOT NULL CHECK(cordinates LIKE '[0-9][0-9]° [0-9][0-9]'' [0-9][0-9]" [NS] [0-9][0-9]° [0-9][0-9]'' [0-9][0-9]" [EW]'),
	height INT NOT NULL
)

INSERT INTO route_points(name, cordinates, height) VALUES ('Œnie¿ka', '50° 44'' 07" N 15° 44'' 23" E', 1603)
INSERT INTO route_points(name, cordinates, height) VALUES ('Dom Œl¹ski', '50° 44'' 22" N 15° 43'' 43" E', 1394)
INSERT INTO route_points(name, description, cordinates, height) VALUES ('Schronisko nad £omniczk¹', 'Niedawno zamkniête schronisko', '50° 44'' 53" N 15° 44'' 38" E', 1002)

CREATE TABLE routes (
	id INT IDENTITY PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	length DECIMAL NOT NULL,
	sumOfClimbs INT NOT NULL,
	mountainGroupId INT FOREIGN KEY REFERENCES mountain_group(id),
	startPointId INT FOREIGN KEY REFERENCES route_points(id) NOT NULL,
	endPointId INT FOREIGN KEY REFERENCES route_points(id) NOT NULL,
	UNIQUE (name, startPointId, endPointId)
)

INSERT INTO routes(name, length, sumOfClimbs, mountainGroupId, startPointId, endPointId) VALUES ('Trasa Stulecia', 2.5, 356, (SELECT id FROM mountain_group WHERE name = 'Beskidy Œl¹sk'), 2, 1)
INSERT INTO routes(name, length, sumOfClimbs, mountainGroupId, startPointId, endPointId) VALUES ('Trasa jubileuszowa', 1.23, 500, (SELECT id FROM mountain_group WHERE name = 'Beskidy Œl¹sk'), 2, 1)
INSERT INTO routes(name, length, sumOfClimbs, mountainGroupId, startPointId, endPointId) VALUES ('Trasa Stulecia', 2.5, 0, (SELECT id FROM mountain_group WHERE name = 'Tatry wysokie'), 1, 2)
INSERT INTO routes(name, length, sumOfClimbs, mountainGroupId, startPointId, endPointId) VALUES ('Trasa jubileuszowa', 1.23, 0, (SELECT id FROM mountain_group WHERE name = 'Beskidy Œl¹sk'), 1, 2)
INSERT INTO routes(name, length, sumOfClimbs, mountainGroupId, startPointId, endPointId) VALUES ('W ko³o slê¿y', 10, 100, (SELECT id FROM mountain_group WHERE name = 'Beskid Niski'), 3, 3)

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
	difficulty VARCHAR(50) NOT NULL CHECK(difficulty IN ('Easy', 'Moderate', 'Hard', 'Extreme')),
	CHECK(openingDate <= closingDate),
)

INSERT INTO standard_routes_data VALUES ((SELECT MIN(id) FROM routes WHERE name = 'Trasa Stulecia'), '2000-01-01', NULL, 75, 'Easy')
INSERT INTO standard_routes_data VALUES ((SELECT MIN(id) FROM routes WHERE name = 'Trasa jubileuszowa'), '2010-01-01', NULL, 45, 'Moderate')
INSERT INTO standard_routes_data VALUES ((SELECT MAX(id) FROM routes WHERE name = 'Trasa Stulecia'), '2000-01-01', NULL, 75, 'Easy')
INSERT INTO standard_routes_data VALUES ((SELECT MAX(id) FROM routes WHERE name = 'Trasa jubileuszowa'), '2010-01-01', NULL, 45, 'Easy')

CREATE TABLE badges (
	id INT IDENTITY PRIMARY KEY,
	type VARCHAR(20) NOT NULL CHECK (type IN('IntoMountains', 'Popular', 'Small', 'Big', 'ForPersistance'))
)

INSERT INTO badges(type) VALUES ('IntoMountains')
INSERT INTO badges(type) VALUES ('Popular')
INSERT INTO badges(type) VALUES ('Small')
INSERT INTO badges(type) VALUES ('Big')
INSERT INTO badges(type) VALUES ('ForPersistance')

CREATE TABLE badge_ranks(
	id INT IDENTITY PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	quota INT,
	badgeId INT FOREIGN KEY REFERENCES badges(id) NOT NULL
)

INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Br¹zowy', 15, (SELECT id FROM badges WHERE type = 'IntoMountains'))
INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Srebny', 30, (SELECT id FROM badges WHERE type = 'IntoMountains'))
INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Z³oty', 45, (SELECT id FROM badges WHERE type = 'IntoMountains'))
INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Popularny', 60, (SELECT id FROM badges WHERE type = 'Popular'))
INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Br¹zowy', 120, (SELECT id FROM badges WHERE type = 'Small'))
INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Srebny', 360, (SELECT id FROM badges WHERE type = 'Small'))
INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Z³oty', 720, (SELECT id FROM badges WHERE type = 'Small'))
INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Br¹zowy', NULL, (SELECT id FROM badges WHERE type = 'Big'))
INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Srebny', NULL, (SELECT id FROM badges WHERE type = 'Big'))
INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Z³oty', NULL, (SELECT id FROM badges WHERE type = 'Big'))
INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Ma³y', NULL, (SELECT id FROM badges WHERE type = 'ForPersistance'))
INSERT INTO badge_ranks(name, quota, badgeId) VALUES ('Du¿y', NULL, (SELECT id FROM badges WHERE type = 'ForPersistance'))

CREATE TABLE tours (
	id INT IDENTITY PRIMARY KEY,
	entryDate DATE DEFAULT GETDATE() NOT NULL,
	turistId INT FOREIGN KEY REFERENCES turists_data(userId) NOT NULL
)

INSERT INTO tours(entryDate, turistId) VALUES ('05-09-2020', (SELECT userId FROM turists_data WHERE sureName = 'Ma³ysz'))
INSERT INTO tours(entryDate, turistId) VALUES ('09-20-2020', (SELECT userId FROM turists_data WHERE sureName = 'Ma³ysz'))
INSERT INTO tours(entryDate, turistId) VALUES ('07-15-2020', (SELECT userId FROM turists_data WHERE sureName = 'Skalisty'))

CREATE TABLE entries (
	id INT IDENTITY PRIMARY KEY,
	dateOfPassing DATE DEFAULT GETDATE() NOT NULL,
	verified BIT NOT NULL,
	routeId INT FOREIGN KEY REFERENCES routes(id) NOT NULL,
	tourId INT FOREIGN KEY REFERENCES tours(id) NOT NULL
)

INSERT INTO entries(dateOfPassing, verified, routeId, tourId) VALUES ('05-09-2020', 1, 1, (SELECT id FROM tours WHERE entryDate = '05-09-2020'))
INSERT INTO entries(dateOfPassing, verified, routeId, tourId) VALUES ('05-09-2020', 1, 3, (SELECT id FROM tours WHERE entryDate = '05-09-2020'))
INSERT INTO entries(dateOfPassing, verified, routeId, tourId) VALUES ('09-20-2019', 1, 5, (SELECT id FROM tours WHERE entryDate = '09-20-2020'))
INSERT INTO entries(dateOfPassing, verified, routeId, tourId) VALUES ('07-15-2020', 1, 3, (SELECT id FROM tours WHERE entryDate = '07-15-2020'))

CREATE TABLE badge_applications (
	id INT IDENTITY PRIMARY KEY,
	awardDate DATE,
	status VARCHAR(20) NOT NULL CHECK(status IN ('Approved', 'Rejected', 'InProgress')),
	description VARCHAR(500),
	rankId INT FOREIGN KEY REFERENCES badge_ranks(id) NOT NULL,
	leaderId INT FOREIGN KEY REFERENCES leader_data(userId) NOT NULL,
	turistId INT FOREIGN KEY REFERENCES turists_data(userId) NOT NULL,
	CHECK((status = 'Approved' AND awardDate IS NOT NULL) 
		OR (status = 'Rejected' AND description IS NOT NULL) 
		OR (status = 'InProgress'))
)

INSERT INTO badge_applications(status, description, rankId, leaderId, turistId) VALUES ('InProgress', 'Pierwsza odznaka', (SELECT id FROM badge_ranks WHERE quota = 60), 
														(SELECT id FROM users WHERE login = 'przodownik2'), 
														(SELECT userId FROM turists_data WHERE sureName = 'Ma³ysz'))
INSERT INTO badge_applications(status, awardDate, rankId, leaderId, turistId) VALUES ('Approved', '07-07-2019', (SELECT id FROM badge_ranks WHERE quota = 60), 
														(SELECT id FROM users WHERE login = 'przodownik2'), 
														(SELECT userId FROM turists_data WHERE sureName = 'Skalisty'))
INSERT INTO badge_applications(status, rankId, leaderId, turistId) VALUES ('InProgress', (SELECT id FROM badge_ranks WHERE quota = 120), 
														(SELECT id FROM users WHERE login = 'przodownik2'), 
														(SELECT userId FROM turists_data WHERE sureName = 'Skalisty'))


CREATE TABLE badge_application_tours (
	badgeApplicationId INT FOREIGN KEY REFERENCES badge_applications(id),
	tourId int FOREIGN KEY REFERENCES mountain_group(id),
	PRIMARY KEY(badgeApplicationId, tourId)
)

INSERT INTO badge_application_tours VALUES (1, 1)
INSERT INTO badge_application_tours VALUES (1, 2)
INSERT INTO badge_application_tours VALUES (3, 3)
