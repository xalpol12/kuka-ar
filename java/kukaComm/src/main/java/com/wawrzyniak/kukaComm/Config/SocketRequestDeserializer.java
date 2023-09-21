package com.wawrzyniak.kukaComm.Config;

import com.fasterxml.jackson.core.JacksonException;
import com.fasterxml.jackson.core.JsonParser;
import com.fasterxml.jackson.databind.DeserializationContext;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.deser.std.StdDeserializer;
import com.wawrzyniak.kukaComm.Model.SocketRequest.DataRequest;
import com.wawrzyniak.kukaComm.Model.SocketRequest.SocketRequest;
import com.wawrzyniak.kukaComm.Model.SocketRequest.UnsubscribeRequest;
import com.wawrzyniak.kukaComm.Model.Types.VarType;

import java.io.IOException;

public class SocketRequestDeserializer extends StdDeserializer<SocketRequest> {

    protected SocketRequestDeserializer(Class<?> vc) {
        super(vc);
    }

    public SocketRequestDeserializer(){
        this(null);
    }
    @Override
    public SocketRequest deserialize(JsonParser jsonParser, DeserializationContext deserializationContext) throws IOException, JacksonException {
        JsonNode node = jsonParser.getCodec().readTree(jsonParser);
        if (node.get("host") != null) {
            VarType var = null;
            switch (node.get("var").asText()) {
                case "BASE" -> var = VarType.BASE;
                case "POSITION" -> var = VarType.POSITION;
                case "BASE_NUMBER" -> var = VarType.BASE_NUMBER;
                case "TOOL_NUMBER" -> var = VarType.TOOL_NUMBER;
                case "JOINTS" -> var = VarType.JOINTS;
            }
            return new DataRequest(node.get("host").asText(), var);
        }
        if (node.get("unsubscribeIp") != null){
            return new UnsubscribeRequest(node.get("unsubscribeIp").asText());
        }
        return null;
    }
}
