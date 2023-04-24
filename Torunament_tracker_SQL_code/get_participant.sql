CREATE DEFINER=`root`@`localhost` PROCEDURE `get_participant`(
	participant_id tinyint
)
begin
	select 
		first_name,
        last_name,
        email,
        phone
	from participants p
    where p.participant_id = participant_id;
end