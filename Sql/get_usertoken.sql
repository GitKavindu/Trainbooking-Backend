CREATE OR REPLACE FUNCTION public.get_usertoken(
    _token_id character varying,
    start_time timestamp without time zone,
    end_time timestamp without time zone,
    _username character varying)
RETURNS character varying
LANGUAGE plpgsql
AS $BODY$
DECLARE
    new_token_id character varying;
BEGIN
    -- Insert the token into the token table
    INSERT INTO token (token_id, username, received_time, end_time)
    VALUES (_token_id, _username, start_time, end_time)
    RETURNING token_id INTO new_token_id;  -- Capture the token_id of the new row

    -- Return the new_token_id
    RETURN new_token_id;
END;
$BODY$;


