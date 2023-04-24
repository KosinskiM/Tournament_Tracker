CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_participant`(
    first_name varchar(50),
    last_name varchar(50),
    email varchar(255),
    phone varchar(100),
    out participant_id int
)
begin
	insert into participants(first_name,last_name,email,phone)
    values(first_name,last_name,email,phone);
    
	select count(*)
    into participant_id
	from participants;
end