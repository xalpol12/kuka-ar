package com.wawrzyniak.testsocket.Config;

import com.fasterxml.jackson.core.JacksonException;
import com.fasterxml.jackson.core.JsonParser;
import com.fasterxml.jackson.databind.DeserializationContext;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.deser.std.StdDeserializer;
import com.wawrzyniak.testsocket.Model.Request.DataRequest;
import com.wawrzyniak.testsocket.Model.Request.SocketRequest;
import com.wawrzyniak.testsocket.Model.Request.UnsubscribeRequest;
import com.wawrzyniak.testsocket.Model.Types.VarType;

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
            VarType var = VarType.WRONG;
            switch (node.get("var").asText()) {
                case "BASE" -> var = VarType.BASE;
                case "POSITION" -> var = VarType.POSITION;
                case "BASE_NUMBER" -> var = VarType.BASE_NUMBER;
                case "TOOL_NUMBER" -> var = VarType.TOOL_NUMBER;
                case "JOINTS" -> var = VarType.JOINTS;
                case "WRONG" -> var = VarType.WRONG;
            }
            return new DataRequest(node.get("host").asText(), var);
        }
        if (node.get("unsubscribeIp") != null){
            return new UnsubscribeRequest(node.get("unsubscribeIp").asText());
        }
        return null;
    }
}
