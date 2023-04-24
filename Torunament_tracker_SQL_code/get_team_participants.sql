CREATE DEFINER=`root`@`localhost` PROCEDURE `get_team_participants`(
	team_id tinyint
)
begin
	select *
    from participants p
    where p.participant_id in 
	(
    select team_member_id
    from team_members tm
    where tm.team_id = team_id
    group by team_member_id
    );
end