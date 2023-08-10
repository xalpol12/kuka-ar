package com.wawrzyniak.testsocket.Model.Value;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.wawrzyniak.testsocket.Exceptions.EmptyBaseOrToolException;
import com.wawrzyniak.testsocket.Model.Records.Vector3;
import lombok.*;

import java.util.Random;

@Getter
@Setter
@EqualsAndHashCode(callSuper = false)
@AllArgsConstructor
@NoArgsConstructor
public class KRLFrame extends JsonFormatter implements KRLValue {
    private Vector3 position;
    private Vector3 rotation;

    public void setAllValues(double[] values){
        position = new Vector3(values[0], values[1], values[2]);
        rotation = new Vector3(values[5], values[4], values[3]);
    }
    @Override
    public void setValueFromString(String values) throws EmptyBaseOrToolException {
        String[] dataToSplit = values.split(":");
        if(dataToSplit.length < 2){
            throw new EmptyBaseOrToolException("Set tool and base of robot to check position of either");
        }
        String data = dataToSplit[1];
        data = data.substring(0, data.length()-1);
        String[] valueToParse = data.split(",");
        double[] dataForFrame = new double[valueToParse.length];
        for (int i = 0; i < valueToParse.length; i++){
            String[] splittedValue = valueToParse[i].trim().split(" ");
            if(splittedValue.length < 2){
                throw new EmptyBaseOrToolException("Set tool and base of robot to check position of either");
            }
            dataForFrame[i] = Double.parseDouble(splittedValue[1]);
        }
        setAllValues(dataForFrame);
    }

    @Override
    public String toJsonString() throws JsonProcessingException {
        return this.toJson();
    }

    @Override
    public void setRandomValues() {
        Random rand = new Random();
        position = new Vector3(rand.nextDouble(50, 250), rand.nextDouble(50, 250), rand.nextDouble(50, 250));
        rotation = new Vector3(rand.nextDouble(50, 250), rand.nextDouble(50, 250), rand.nextDouble(50, 250));
    }
}
