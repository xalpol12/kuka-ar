package com.wawrzyniak.testsocket.Model;

import com.wawrzyniak.testsocket.Exceptions.EmptyBaseOrToolException;
import com.wawrzyniak.testsocket.Model.Records.ExceptionMessagePair;
import com.wawrzyniak.testsocket.Model.Types.VarType;
import com.wawrzyniak.testsocket.Model.Value.KRLValue;
import com.wawrzyniak.testsocket.Service.IdProvider;
import lombok.Getter;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class KRLVar {
    KRLValue value;

    @Getter
    private final int id;
    @Getter
    private final String name;
    @Getter
    private Byte[] readRequest;
    private Set<ExceptionMessagePair> readExceptions;

    public KRLVar(VarType var){
        this.name = var.getValue().name();
        this.value = var.getValue().type();
        this.id = IdProvider.getId();
        generateReadRequest();
        readExceptions = new HashSet<>();
    }

    public void addReadException(Exception exception){
        readExceptions.add(new ExceptionMessagePair(exception.getClass().getSimpleName(), exception.getMessage()));
    }

    public void clearExceptions(){
        readExceptions.clear();
    }

    public Set<ExceptionMessagePair> getReadExceptions(){
        return this.readExceptions;
    }

    public KRLValue getValue(){
        return value;
    }

    public boolean idCheck(int id){
        return this.id == id;
    }

    public void setValue(String read) throws EmptyBaseOrToolException {
        value.setValueFromString(read);
    }

    private void generateReadRequest(){
        List<Byte> body = generateReadBody();
        List<Byte> header = generateHeader(body.size());
        header.addAll(body);
        this.readRequest = header.toArray(new Byte[0]);
    }

    private List<Byte> generateReadBody(){
        List<Byte> body = new ArrayList<>();
        byte[] command = name.getBytes();
        int length = command.length;
        byte highLength = (byte) (length & 0x00ff);
        byte lowLength = (byte) (length & 0xff00);
        body.add((byte) 0);
        body.add(highLength);
        body.add(lowLength);
        for (byte data : command){
            body.add(data);
        }
        return body;
    }

    private List<Byte> generateHeader(int length){
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

    public void setRandomValues(){
        value.setRandomValues();
    }
    
    public KRLValue setValue(KRLValue value){
        this.value = value;
        return this.value;
    }
}
