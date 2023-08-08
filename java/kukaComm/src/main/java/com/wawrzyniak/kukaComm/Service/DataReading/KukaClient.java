package com.wawrzyniak.kukaComm.Service.DataReading;

import com.wawrzyniak.kukaComm.Exceptions.EmptyBaseOrToolException;
import com.wawrzyniak.kukaComm.Exceptions.WrongIdException;
import com.wawrzyniak.kukaComm.Model.KRLVar;
import com.wawrzyniak.kukaComm.Model.Types.VarType;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.IOException;
import java.net.Socket;
import java.util.HashMap;
import java.util.Map;

public class KukaClient extends Socket {
    private final BufferedInputStream input;
    private final BufferedOutputStream output;
    private final Map<VarType, KRLVar> variables;

    public KukaClient(String host) throws IOException {
        super(host, 7000);
        input = new BufferedInputStream(getInputStream());
        output = new BufferedOutputStream(getOutputStream());
        variables = new HashMap<>();
    }

    public void addVariable(VarType var) {
        if(!variables.containsKey(var)) {
            variables.put(var, new KRLVar(var));
        }
    }

    void readVar(KRLVar var) {
        try {
            for (byte data : var.getReadRequest()) {
                output.write(data);
            }
            output.flush();
            readResponse(var);
        } catch (WrongIdException | IOException | EmptyBaseOrToolException e) {
            var.addReadException(e);
        }
    }

    private void readResponse(KRLVar var) throws IOException, EmptyBaseOrToolException, WrongIdException {
        byte[] header = new byte[7];
        input.read(header);
        byte[] body = new byte[getIntFromBytes(header, 2)];
        input.read(body);
        int id = getIntFromBytes(header, 0);
        String readValue = new String(body).trim();
        if(!var.idCheck(id)) {
            throw new WrongIdException("Id of received message is not matching id of send message");
        }
        var.setValue(readValue);
    }
    private int getIntFromBytes(byte[] bytes, int off) {
        return bytes[off] << 8 & 0xFF00 | bytes[off + 1] & 0xFF;
    }

    public KRLVar getVariable(VarType var){
        return variables.get(var);
    }
}
