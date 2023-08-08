package com.wawrzyniak.kukaComm.Model.Value;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

public class JsonFormatter {
    private final ObjectMapper mapper = new ObjectMapper();

    protected String toJson() throws JsonProcessingException {
        return mapper.writeValueAsString(this);
    }
}
