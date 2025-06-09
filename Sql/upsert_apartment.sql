CREATE OR REPLACE PROCEDURE public.upsert_apartment(
	_is_update bool,
	_apartment_id int,
	_class VARCHAR(50),
	_train_id int,
	_train_seq_no int,
	_is_active bool,
	_added_by VARCHAR(50),
	OUT _result integer
)
LANGUAGE 'plpgsql'
AS $BODY$
DECLARE
	v_apartment_id INT;
	v_created_date TIMESTAMP;
	v_max_seq_no INT;
BEGIN

	--check whether the entered train id is valid
	IF _train_id IS NULL OR _train_seq_no IS NULL THEN
		RAISE EXCEPTION 'Train id or seq no is null!' USING ERRCODE = '45000';
	ELSIF NOT EXISTS (SELECT 1 FROM train WHERE train_no=_train_id AND seq_no=_train_seq_no AND is_active=true) THEN
		--raise exception for train do not exist
		RAISE EXCEPTION 'Train do not exist!' USING ERRCODE = '45000';
	END IF;
	
	IF _is_update THEN
				
		--select 1 for given apartment id to check it exists
		IF EXISTS (SELECT 1 FROM apartments WHERE apartment_id=_apartment_id AND is_active=true) THEN
			
			--disable  apartment for this id
			UPDATE apartments 
			SET is_active=false
			WHERE apartment_id=_apartment_id;
			
			IF _is_active THEN
				--update apartment
			
				--select created date
				SELECT added_date INTO v_created_date
				FROM apartments
				WHERE apartment_id=_apartment_id;
				
				--update apartment
				INSERT INTO apartments (class,added_by,added_date,modified_date,is_active,train_seq_no,train_id,updated_from)
				VALUES (_class,_added_by,v_created_date,(SELECT CURRENT_TIMESTAMP),true,_train_seq_no,_train_id,_apartment_id)
				RETURNING apartment_id INTO _result;
				
			ELSE
				--disable apartment
				--do nothing already disabled
				RAISE NOTICE 'Apartment is disabled. No update needed.' ;
			END IF;
		ELSE
			--raise exception for apartment not exist
			RAISE EXCEPTION 'Apartment do not exist!' USING ERRCODE = '45000';
		END IF;
	ELSE
		--Add a new apartment
		INSERT INTO apartments (class,added_by,added_date,modified_date,is_active,train_seq_no,train_id,updated_from)
		VALUES (_class,_added_by,(SELECT CURRENT_TIMESTAMP),null,true,_train_seq_no,_train_id,null)
		RETURNING apartment_id INTO _result;
	END IF;
END;
$BODY$;