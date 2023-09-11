package com.wawrzyniak.testsocket.Config;

import com.fasterxml.jackson.core.JacksonException;
import com.fasterxml.jackson.core.JsonParser;
import com.fasterxml.jackson.databind.DeserializationContext;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.deser.std.StdDeserializer;
import com.wawrzyniak.testsocket.Model.Records.Vector3;
import com.wawrzyniak.testsocket.Model.Value.KRLFrame;
import com.wawrzyniak.testsocket.Model.Value.KRLInt;
import com.wawrzyniak.testsocket.Model.Value.KRLJoints;
import com.wawrzyniak.testsocket.Model.Value.KRLValue;

import java.io.IOException;

public class KRLValueDeserializer extends StdDeserializer<KRLValue> {

    protected KRLValueDeserializer(Class<?> vc) {
        super(vc);
    }

    public KRLValueDeserializer(){
        this(null);
    }

    @Override
    public KRLValue deserialize(JsonParser jsonParser, DeserializationContext deserializationContext) throws IOException, JacksonException {
        JsonNode node = jsonParser.getCodec().readTree(jsonParser);
        if(node.get("rotation") != null){
            JsonNode rotation = node.get("rotation");
            JsonNode position = node.get("position");
            Vector3 rotationPOJO = new Vector3(rotation.get("x").asInt(), rotation.get("y").asInt(), rotation.get("z").asInt());
            Vector3 positionPOJO = new Vector3(position.get("x").asInt(), position.get("y").asInt(), position.get("z").asInt());
            return new KRLFrame(positionPOJO, rotationPOJO);
        }
        if(node.get("valueInt") != null) {
            return new KRLInt(node.get("valueInt").asInt());
        }
        if(node.get("j1") != null){
            return new KRLJoints(node.get("j1").asDouble(),
                    node.get("j2").asDouble(),
                    node.get("j3").asDouble(),
                    node.get("j4").asDouble(),
                    node.get("j5").asDouble(),
                    node.get("j6").asDouble());
        }
        return null;
    }
}
