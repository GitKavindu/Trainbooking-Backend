-- FUNCTION: public.authenticate_user(character varying)

-- DROP FUNCTION IF EXISTS public.authenticate_user(character varying);

CREATE OR REPLACE FUNCTION public.authenticate_user(
	_token_id character varying)
    RETURNS SETOF token_details 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
DECLARE
    local_time TIMESTAMP;
	end_token_time TIMESTAMP;
BEGIN
    SELECT LOCALTIMESTAMP into local_time;
	select end_time into end_token_time from token where token_id=_token_id;
	
	RAISE NOTICE 'The value of my_variable is: %', end_token_time;
	RAISE NOTICE 'The value of your_variable is: %', local_time;
	
    IF end_token_time is not null and end_token_time > local_time THEN
        RETURN QUERY
		select "t".is_active,"u".username,"u".is_active , "u".is_admin 
		from users "u"
		INNER join token "t" ON "t".username = "u".username
		where "t".token_id=_token_id;
		--result := 'Positive';
	ELSE
		RETURN QUERY
		select false ,'None'::varchar(50),false, false;
    END IF;	

	
END;
$BODY$;

ALTER FUNCTION public.authenticate_user(character varying)
    OWNER TO postgres;
