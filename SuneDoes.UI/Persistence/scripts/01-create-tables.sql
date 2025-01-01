create table email_address (
    email_address_id bigint primary key generated always as identity,
    email_address_string varchar(200) not null,
    code_string varchar(200) not null,
    last_verification_mail_sent timestamp,
    verified_at timestamp,
    unique(email_address_string)
);


create table medicine_notification (
    notification_id bigint primary key generated always as identity,
    email_address varchar(200) not null,
    notify_type varchar(200) not null,
    medicine_type varchar(200) not null,
    notification_time timestamp,
    notification_comment varchar(2000),
    unique(email_address, notify_type)
);
