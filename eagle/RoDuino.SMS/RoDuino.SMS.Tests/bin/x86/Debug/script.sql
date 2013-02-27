
    drop table if exists Users

    drop table if exists Clients

    drop table if exists SmsHistory

    drop table if exists ModemSettings

    create table Users (
        Id  integer,
       Name TEXT,
       Username TEXT,
       Password TEXT,
       primary key (Id)
    )

    create table Clients (
        Id  integer,
       Name TEXT,
       Phone TEXT,
       Network TEXT,
       Email TEXT,
       IsDeleted INTEGER,
       primary key (Id)
    )

    create table SmsHistory (
        Id  integer,
       Name TEXT,
       ClientName TEXT,
       ClientPhone TEXT,
       Message TEXT,
       Date TEXT,
       Status TEXT,
       primary key (Id)
    )

    create table ModemSettings (
        Id  integer,
       Name TEXT,
       Port TEXT,
       BitPerSec INTEGER,
       DataBits INTEGER,
       Parity INTEGER,
       StopBits INTEGER,
       FlowControl TEXT,
       primary key (Id)
    )
