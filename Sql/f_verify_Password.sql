CREATE OR REPLACE FUNCTION verify_Password(user_name VARCHAR(50),user_password VARCHAR(300)) 
RETURNS BOOLEAN AS $$
DECLARE
    _password VARCHAR(300);
BEGIN
    
    SELECT password INTO _password
	FROM users
    WHERE username=user_name;

	-- Check if the retrieved password matches the given password
    IF _password = user_password THEN
        RETURN TRUE;  -- Return TRUE if passwords match
    ELSE
        RETURN FALSE;  -- Return FALSE if passwords do not match
    END IF;
END;
$$ LANGUAGE plpgsql;