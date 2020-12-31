DROP TABLE IF EXISTS turists_data
DROP TABLE IF EXISTS users

CREATE TABLE users (
	id int IDENTITY PRIMARY KEY,
	login VARCHAR(40) UNIQUE,
	password CHAR(48) NOT NULL,
	email VARCHAR(50) NOT NULL,
	isAdmin BIT NOT NULL DEFAULT 0,
	joinDate DATETIME NOT NULL DEFAULT GETDATE()
)

INSERT INTO users(isadmin, login, email, password) VALUES (1, 'admin', 'admin@gmail.com', '/gFdKiSaODPcyZNwBipWbNJAStwhrnfCLhFYMzBon+zoOP1E') --haslo: admin
INSERT INTO users(login, email, password) VALUES ('turysta', 'turysta.zmeczony@gmail.com', 'Ptqk1oarroB94b/hHV9PkI/XlHHsEsduU5IAeoJ2QuxCFdDy') --haslo: turysta123
INSERT INTO users(login, email, password) VALUES ('przodownik', 'przod.najlepszy@gmail.com', 'VeZsvu0K/rFI8P08z5ITR/mO4WfPlu2wovQMUrhhZqj1tgVV') --haslo: przodownik
INSERT INTO users(login, email, password) VALUES ('franek', 'franek.test@gmail.com', 'vCp46N/R5p9p9mkomAcJLa407E7MjsSu/X3c6kq+44Jqw8hV') --haslo: qwerty

CREATE TABLE turists_data (
	userId int PRIMARY KEY FOREIGN KEY REFERENCES users(id),
	firstName VARCHAR(50),
	sureName VARCHAR(50)
)

INSERT INTO turists_data VALUES((SELECT id FROM users WHERE login = 'turysta'), 'Adam', 'Ma³ysz')