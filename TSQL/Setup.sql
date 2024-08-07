--use master;

--create database Chat_app;

use Chat_app;

create table Users (
	user_id int primary key identity(1,1),
	user_tag varchar(255) not null,
	first_name varchar(255) not null,
	last_name varchar(255) not null,
	country varchar(255),
	date_of_birth date,
	username varchar(255) not null,
	email varchar(255) not null,
	hashed_password varchar(255) not null,
	created_at datetime not null,
)

create table Status_enum (
	status_id int primary key identity(1,1),
	status_name varchar(15) not null,
)

insert into status_enum (status_name) values
('Accepted'),
('Pending'),
('Rejected');

create table Friendships (
	friend_ship_id int primary key identity(1,1),
	user_1_id int not null,
	user_2_id int not null,
	status_id int not null,
	created_at datetime not null,

	foreign key (user_1_id) references Users(user_id),
	foreign key (user_2_id) references Users(user_id),
	foreign key (status_id) references Status_enum(status_id),
)

create table Chats (
	chat_id int primary key identity(1,1),
	from_user int not null,
	to_user int not null,
	pinned tinyint not null,
	user_id int not null,	
	created_at datetime not null,

	foreign key (user_id) references Users(user_id),
	foreign key (from_user) references Users(user_id),
	foreign key (to_user) references Users(user_id),
)

INSERT INTO Users(
    user_id,
	first_name,
	last_name,
	country,
	date_of_birth,
	username,
	email,
	hashed_password,
	created_at,
)
values (
    'TEST',
    'TEST',
    'dk',
    TO_DATE('18/06/12', 'DD/MM/YYYY'),
    'TEST',
    'TEST@gmail.com',
    'vhvhjen4yuet67iw7nivyvn4iwru',
    '07082024 10:58:09 AM',
)