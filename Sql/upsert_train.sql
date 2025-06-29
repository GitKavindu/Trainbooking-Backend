CREATE OR REPLACE PROCEDURE public.upsert_train(
	_is_update bool,
	_train_id int,
	_train_name VARCHAR(50),
	_is_active bool,
	_added_by VARCHAR(50)
)
LANGUAGE 'plpgsql'
AS $BODY$
DECLARE
	v_train_id INT;
	v_created_date TIMESTAMP;
	v_max_seq_no INT;
BEGIN
	IF _is_update THEN
		--select 1 for given train_id to check it exists
		IF EXISTS (SELECT 1 FROM train WHERE train_no=_train_id AND is_active=true) THEN
			
			--disable all trains for this id
			UPDATE train 
			SET is_active=false
			WHERE train_no=_train_id AND is_active=true;
			
			IF _is_active THEN
				--update station
				
				--select created date
				SELECT added_date INTO v_created_date
				FROM train
				WHERE train_no=_train_id AND seq_no=1;

				--SELECT MAX SEQ NO for  a ceratin train
				SELECT MAX(seq_no) INTO v_max_seq_no
				FROM train
				WHERE train_no=_train_id;
				
				--update train
				INSERT INTO train (train_no,name,added_by,added_date,modified_date,is_active,seq_no)
				VALUES (_train_id,_train_name,_added_by,v_created_date,(SELECT CURRENT_TIMESTAMP),true,(v_max_seq_no+1)) ;
				
			ELSE
				--disable train
				--do nothing already disabled
				RAISE NOTICE 'Train is disabled. No update needed.' ;
			END IF;
		ELSE
			--raise exception for train not exist
			RAISE EXCEPTION 'Train do not exist!' USING ERRCODE = '45000';
		END IF;
	ELSE
		--Add a new train
		INSERT INTO train (name,added_by,added_date,modified_date,is_active,seq_no)
		VALUES (_train_name,_added_by,(SELECT CURRENT_TIMESTAMP),null,true,1);
	END IF;
END;
$BODY$;