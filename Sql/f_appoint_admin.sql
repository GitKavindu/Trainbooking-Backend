-- FUNCTION: public.appoint_admin(character varying, character varying, character varying)

-- DROP FUNCTION IF EXISTS public.appoint_admin(character varying, character varying, character varying);

CREATE OR REPLACE FUNCTION public.appoint_admin(
	_appointed_by character varying,
	_username character varying,
	_position character varying,
	_do_appoint BOOLEAN
	)
    RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
BEGIN
    RAISE NOTICE 'The value of _do_appoint is: %', _do_appoint;	
    
	IF _do_appoint=true THEN
	
		-- First update users table
	    UPDATE users set is_admin=true where username=_username;
		
	    -- Second insert data to admin table
	    INSERT INTO admin (appointed_by,position,username) VALUES (_appointed_by,_position,_username);
    ELSE
       	-- First update users table
	    UPDATE users set is_admin=false where username=_username;
		
	    -- Second delete data from admin table
	    DELETE FROM admin where username=_username;
    END IF;
	
    
	EXCEPTION WHEN OTHERS THEN
    -- If there's an error, rollback the transaction
    ROLLBACK;
    RAISE;

END;
$BODY$;

ALTER FUNCTION public.appoint_admin(character varying, character varying, character varying)
    OWNER TO postgres;
