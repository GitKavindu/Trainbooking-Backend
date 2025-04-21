CREATE OR REPLACE PROCEDURE insert_user_names
(
	_seq_no int,
	_username varchar(50),
	_name varchar(50)
)
LANGUAGE plpgsql
AS $$
BEGIN
	insert into name(name,seq_no,username)
	values(_name,_seq_no,_username);
END;
$$;