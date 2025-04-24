CREATE OR REPLACE PROCEDURE public.upsert_station(
	_is_update bool,
	_station_id int,
	_station_name VARCHAR(50),
	_is_active bool,
	_added_by VARCHAR(50)
)
LANGUAGE 'plpgsql'
AS $BODY$
DECLARE
	v_station_id INT;
	v_created_date TIMESTAMP;
	v_max_seq_no INT;
BEGIN
	IF _is_update THEN
		--select 1 for given station_id to check it exists
		IF EXISTS (SELECT 1 FROM station WHERE station_id=_station_id AND is_active=true) THEN
			
			--disable all stations for this id
			UPDATE station 
			SET is_active=false
			WHERE station_id=_station_id;
			
			IF _is_active THEN
				--update station
				
				--select created date
				SELECT added_date INTO v_created_date
				FROM station
				WHERE station_id=_station_id AND seq_no=1;

				--SELECT MAX SEQ NO for  a ceratin station
				SELECT MAX(seq_no) INTO v_max_seq_no
				FROM station
				WHERE station_id=_station_id;

				--update station
				INSERT INTO station (station_id,station_name,added_by,added_date,modified_date,is_active,seq_no)
				VALUES (_station_id,_station_name,_added_by,v_created_date,(SELECT CURRENT_TIMESTAMP),true,(v_max_seq_no+1)) ;
				
			ELSE
				--disable station
				--do nothing already disabled
				RAISE NOTICE 'Station is disabled. No update needed.' ;
			END IF;
		ELSE
			--raise exception for station not exist
			RAISE EXCEPTION 'Station do not exist!' USING ERRCODE = '45000';
		END IF;
	ELSE
		--Add a new station
		INSERT INTO station (station_name,added_by,added_date,modified_date,is_active,seq_no)
		VALUES (_station_name,_added_by,(SELECT CURRENT_TIMESTAMP),null,true,1);
	END IF;
END;
$BODY$;