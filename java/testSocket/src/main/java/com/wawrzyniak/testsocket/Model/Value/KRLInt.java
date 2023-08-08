package com.wawrzyniak.testsocket.Model.Value;

import com.fasterxml.jackson.core.JsonProcessingException;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.Setter;

import java.util.concurrent.ThreadLocalRandom;

@Getter
@Setter
@EqualsAndHashCode(callSuper = false)
public class KRLInt extends JsonFormatter implements KRLValue {

    private Integer valueInt;

    @Override
    public void setValueFromString(String values) {
        this.valueInt = Integer.parseInt(values.trim());
    }

    @Override
    public String toJsonString() throws JsonProcessingException {
        return this.toJson();
    }

    @Override
    public void setRandomValues() {
        this.valueInt = ThreadLocalRandom.current().nextInt(0, 33);
    }
}
