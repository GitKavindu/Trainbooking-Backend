CREATE OR REPLACE PROCEDURE insert_user
(
	_username varchar(50),
	_mobile_no varchar(10),
	_email varchar(50),
	_national_id varchar(12),
	_password varchar(300),
	_is_admin BOOLEAN,
	_prefered_name varchar(50)
)
LANGUAGE plpgsql
AS $$
BEGIN
	insert into users(username,mobile_no,email,national_id,password,is_admin,prefered_name) 
	values(_username,_mobile_no,_email,_national_id,_password,_is_admin,_prefered_name);
END;
$$;