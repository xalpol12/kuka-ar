package com.wawrzyniak.testsocket.Model.Value;

import com.fasterxml.jackson.core.JsonProcessingException;
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
import lombok.*;
=======
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.Setter;
>>>>>>> add testSocket and kukaComm
=======
import lombok.*;
>>>>>>> Feature/testsocket setting data (#2)

import java.util.concurrent.ThreadLocalRandom;

@Getter
@Setter
@EqualsAndHashCode(callSuper = false)
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
@AllArgsConstructor
@NoArgsConstructor
=======
>>>>>>> add testSocket and kukaComm
=======
@AllArgsConstructor
@NoArgsConstructor
>>>>>>> Feature/testsocket setting data (#2)
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
