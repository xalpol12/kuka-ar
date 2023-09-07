package com.wawrzyniak.testsocket.Controller;

import com.wawrzyniak.testsocket.Exceptions.RobotAlredyConfiguredException;
import com.wawrzyniak.testsocket.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.testsocket.Exceptions.WrongRequestException;
import com.wawrzyniak.testsocket.Model.Records.ExceptionMessagePair;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import java.io.IOException;

@ControllerAdvice
public class ExceptionController {
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

    @ExceptionHandler(value = WrongRequestException.class)
    public ResponseEntity<ExceptionMessagePair> wrongRequest(Exception e){
        ExceptionMessagePair nameMessagePair = new ExceptionMessagePair(
                e.getClass().getSimpleName(),
                e.getMessage());

        return new ResponseEntity<>(nameMessagePair, HttpStatus.BAD_REQUEST);
    }

    @ExceptionHandler(value = RobotAlredyConfiguredException.class)
    public ResponseEntity<ExceptionMessagePair> robotAlreadySaved(Exception e){
        ExceptionMessagePair nameMessagePair = new ExceptionMessagePair(
                e.getClass().getSimpleName(),
                e.getMessage());
        return new ResponseEntity<>(nameMessagePair, HttpStatus.BAD_REQUEST);
    }
}
