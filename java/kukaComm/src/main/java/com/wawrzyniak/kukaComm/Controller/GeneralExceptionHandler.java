package com.wawrzyniak.kukaComm.Controller;


import com.wawrzyniak.kukaComm.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.kukaComm.Exceptions.WrongIpException;
import com.wawrzyniak.kukaComm.Model.Records.ExceptionMessagePair;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;

import java.io.IOException;

@ControllerAdvice
public class GeneralExceptionHandler {

    private static final Logger logger =
            LoggerFactory.getLogger(GeneralExceptionHandler.class);

    @ExceptionHandler(value = IOException.class)
    public ResponseEntity<ExceptionMessagePair> robotModelReading(Exception e) {
        ExceptionMessagePair nameMessagePair = new ExceptionMessagePair(
                e.getClass().getSimpleName(),
                e.getMessage());

        logger.warn("Exception was thrown: {}, {}",
                nameMessagePair.exceptionName(),
                nameMessagePair.exceptionMessage());

        return new ResponseEntity<>(nameMessagePair, HttpStatus.INTERNAL_SERVER_ERROR);
    }

    @ExceptionHandler(value = RobotNotConfiguredException.class)
    public ResponseEntity<ExceptionMessagePair> robotNorConfigured(Exception e) {
        ExceptionMessagePair nameMessagePair = new ExceptionMessagePair(
                e.getClass().getSimpleName(),
                e.getMessage());

        logger.warn("Exception was thrown: {}, {}",
                nameMessagePair.exceptionName(),
                nameMessagePair.exceptionMessage());

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
