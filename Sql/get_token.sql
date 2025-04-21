CREATE OR REPLACE PROCEDURE get_token(
    _token_id VARCHAR(300),
    start_time TIMESTAMP,
    end_time TIMESTAMP,
    _username VARCHAR(50),
    OUT new_token_id VARCHAR(300)  -- OUT parameter to return the token_id
)
AS $$
BEGIN
    -- Insert the token into the token table
    INSERT INTO token (token_id, username, received_time, end_time)
    VALUES (_token_id, _username, start_time, end_time)
    RETURNING token_id INTO new_token_id;  -- Capture the token_id of the new row

END;
$$ LANGUAGE plpgsql;

