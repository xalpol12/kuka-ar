package com.wawrzyniak.kukaComm.Controller;


import com.wawrzyniak.kukaComm.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.kukaComm.Model.Records.ExceptionMessagePair;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;

import java.io.IOException;

@ControllerAdvice
public class GeneralExceptionHandler {

    @ExceptionHandler(value = IOException.class)
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
    public ResponseEntity<ExceptionMessagePair> robotModelReading(Exception e) {
=======
    public ResponseEntity<ExceptionMessagePair> robotModelReading(Exception e){
>>>>>>> add testSocket and kukaComm
=======
    public ResponseEntity<ExceptionMessagePair> robotModelReading(Exception e) {
>>>>>>> add swagger docs, fix some whitespace issues
        ExceptionMessagePair nameMessagePair = new ExceptionMessagePair(
                e.getClass().getSimpleName(),
                e.getMessage());

        return new ResponseEntity<>(nameMessagePair, HttpStatus.INTERNAL_SERVER_ERROR);
    }

    @ExceptionHandler(value = RobotNotConfiguredException.class)
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
    public ResponseEntity<ExceptionMessagePair> robotNorConfigured(Exception e) {
=======
    public ResponseEntity<ExceptionMessagePair> robotNorConfigured(Exception e){
>>>>>>> add testSocket and kukaComm
=======
    public ResponseEntity<ExceptionMessagePair> robotNorConfigured(Exception e) {
>>>>>>> add swagger docs, fix some whitespace issues
        ExceptionMessagePair nameMessagePair = new ExceptionMessagePair(
                e.getClass().getSimpleName(),
                e.getMessage());

        return new ResponseEntity<>(nameMessagePair, HttpStatus.BAD_REQUEST);
    }
}
