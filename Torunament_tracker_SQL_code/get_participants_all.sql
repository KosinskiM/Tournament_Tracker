CREATE DEFINER=`root`@`localhost` PROCEDURE `get_participants_all`(
)
begin
		select
			participant_id as Id,
            first_name as FirstName,
            last_name as LastName,
            email as EmailAdress,
            phone as CellPhoneNumber
        from participants;
end