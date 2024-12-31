create table email_address (
    email_address_id bigint primary key generated always as identity,
    email_address_string varchar(200) not null,
    code_string varchar(200) not null,
    last_verification_mail_sent timestamp,
    verified_at timestamp,
    unique(email_address_string)
);