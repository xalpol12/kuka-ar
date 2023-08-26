package com.wawrzyniak.kukaComm.Controller;


import com.wawrzyniak.kukaComm.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.kukaComm.Exceptions.WrongIpException;
import com.wawrzyniak.kukaComm.Model.Records.ExceptionMessagePair;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;

import java.io.IOException;

@ControllerAdvice
public class GeneralExceptionHandler {

    @ExceptionHandler(value = IOException.class)
    public ResponseEntity<ExceptionMessagePair> robotModelReading(Exception e) {
        ExceptionMessagePair nameMessagePair = new ExceptionMessagePair(
                e.getClass().getSimpleName(),
                e.getMessage());

        return new ResponseEntity<>(nameMessagePair, HttpStatus.INTERNAL_SERVER_ERROR);
    }

    @ExceptionHandler(value = RobotNotConfiguredException.class)
    public ResponseEntity<ExceptionMessagePair> robotNorConfigured(Exception e) {
        ExceptionMessagePair nameMessagePair = new ExceptionMessagePair(
                e.getClass().getSimpleName(),
                e.getMessage());

        return new ResponseEntity<>(nameMessagePair, HttpStatus.BAD_REQUEST);
    }

    @ExceptionHandler(value = WrongIpException.class)
    public ResponseEntity<ExceptionMessagePair> saveRobotFail(Exception e) {
        ExceptionMessagePair nameMessagePair = new ExceptionMessagePair(
                e.getClass().getSimpleName(),
                e.getMessage());

        return new ResponseEntity<>(nameMessagePair, HttpStatus.UNPROCESSABLE_ENTITY);
    }
}
