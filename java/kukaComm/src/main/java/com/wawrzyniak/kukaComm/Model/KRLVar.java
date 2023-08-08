package com.wawrzyniak.kukaComm.Model;

import com.wawrzyniak.kukaComm.Exceptions.EmptyBaseOrToolException;
import com.wawrzyniak.kukaComm.Model.Records.ExceptionMessagePair;
import com.wawrzyniak.kukaComm.Model.Types.VarType;
import com.wawrzyniak.kukaComm.Model.Value.KRLValue;
import com.wawrzyniak.kukaComm.Service.IdProvider;
import lombok.Getter;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

@Getter
public class KRLVar {

    private final KRLValue value;
    private final int id;
    private final String name;
    private Byte[] readRequest;
    private final Set<ExceptionMessagePair> readExceptions;

<<<<<<< refs/remotes/origin/main
    public KRLVar(VarType var) {
=======
    public KRLVar(VarType var){
>>>>>>> add testSocket and kukaComm
        this.name = var.getValue().name();
        this.value = var.getValue().type();
        this.id = IdProvider.getId();
        generateReadRequest();
        readExceptions = new HashSet<>();
    }

<<<<<<< refs/remotes/origin/main
    public void addReadException(Exception exception) {
=======
    public void addReadException(Exception exception){
>>>>>>> add testSocket and kukaComm
        readExceptions.add(new ExceptionMessagePair(
                exception.getClass().getSimpleName(),
                exception.getMessage()
        ));
    }

<<<<<<< refs/remotes/origin/main
    public void clearExceptions() {
        readExceptions.clear();
    }

    public boolean idCheck(int id) {
=======
    public void clearExceptions(){
        readExceptions.clear();
    }

    public boolean idCheck(int id){
>>>>>>> add testSocket and kukaComm
        return this.id == id;
    }

    public void setValue(String read) throws EmptyBaseOrToolException {
        value.setValueFromString(read);
    }

<<<<<<< refs/remotes/origin/main
    private void generateReadRequest() {
=======
    private void generateReadRequest(){
>>>>>>> add testSocket and kukaComm
        List<Byte> body = generateReadBody();
        List<Byte> header = generateHeader(body.size());
        header.addAll(body);
        this.readRequest = header.toArray(new Byte[0]);
    }

    private List<Byte> generateReadBody() {
        List<Byte> body = new ArrayList<>();
        byte[] command = name.getBytes();
        int length = command.length;
        byte highLength = (byte) ((length & 0xff00)>>8);
        byte lowLength = (byte) (length & 0x00ff);
        body.add((byte) 0);
        body.add(highLength);
        body.add(lowLength);
        for (byte data : command) {
            body.add(data);
        }
        return body;
    }

    private List<Byte> generateHeader(int length) {
        List<Byte> header = new ArrayList<>();
        byte highId = (byte) ((id & 0xff00)>>8);
        byte lowId = (byte) (id & 0x00ff);
        byte highLength = (byte) ((length & 0xff00)>>8);
        byte lowLength = (byte) (length & 0x00ff);
        header.add(highId);
        header.add(lowId);
        header.add(highLength);
        header.add(lowLength);
        return header;
    }
}
