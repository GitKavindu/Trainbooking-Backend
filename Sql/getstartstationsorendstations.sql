-- PROCEDURE: public.getstartstationsorendstations(boolean, character varying)

-- DROP PROCEDURE IF EXISTS public.getstartstationsorendstations(boolean, character varying);

CREATE OR REPLACE PROCEDURE public.getstartstationsorendstations(
	IN _is_startlist boolean,
	IN _scheduleid character varying,
	OUT _result character varying)
LANGUAGE 'plpgsql'
AS $BODY$
DECLARE
    v_max_journey_id INT;
BEGIN
    IF _is_startList =false THEN
		
		SELECT json_agg(row_to_json(a))
		INTO _result
		FROM (
				SELECT s.station_id,s.seq_no AS stationSeqNo,s.station_name
				FROM station s
				LEFT JOIN (
					SELECT * FROM journey 
					WHERE schedule_id=_scheduleid
					) j
				ON j.seq_no = s.seq_no AND j.station_no = s.station_id 
				WHERE s.is_active=true AND j.journey_id IS NULL
		) a;
		
    ELSE
        SELECT MAX(journey_id)
        INTO v_max_journey_id
        FROM journey
        WHERE is_active = true AND schedule_id = _scheduleId;

       
            SELECT json_agg(row_to_json(a))
		    INTO _result
		    FROM (
		       	SELECT s.station_id,s.seq_no AS stationSeqNo,s.station_name
				FROM station s
				LEFT JOIN (
					SELECT * FROM journey 
					WHERE schedule_id=_scheduleid
					) j
				ON j.seq_no = s.seq_no AND j.station_no = s.station_id AND j.journey_id!=v_max_journey_id
				WHERE s.is_active=true AND j.journey_id IS NULL
		    ) a;
    END IF;
END;
$BODY$;
ALTER PROCEDURE public.getstartstationsorendstations(boolean, character varying)
    OWNER TO postgres;
