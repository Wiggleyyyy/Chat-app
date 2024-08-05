--use master;

--create database Chat_app;

use Chat_app;

create table Users (
	user_id int primary key identity(1,1),
	first_name varchar(255) not null,
	last_name varchar(255) not null,
	country varchar(255),
	date_of_birth date,
	username varchar(255) not null,
	email varchar(255) not null,
	encrypted_password varchar(255) not null,
	public_key varchar(255) not null,
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
	chat text not null,
	pinned tinyint not null,
	user_id int not null,	
	created_at datetime not null,

	foreign key (user_id) references Users(user_id),
)