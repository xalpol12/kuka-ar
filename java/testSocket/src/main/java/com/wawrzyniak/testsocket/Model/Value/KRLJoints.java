package com.wawrzyniak.testsocket.Model.Value;

import com.fasterxml.jackson.core.JsonProcessingException;
import lombok.*;

import java.util.Random;

@Getter
@Setter
@EqualsAndHashCode(callSuper = false)
@AllArgsConstructor
@NoArgsConstructor
public class KRLJoints extends JsonFormatter implements KRLValue {

    private double j1;
    private double j2;
    private double j3;
    private double j4;
    private double j5;
    private double j6;

    @Override
    public void setValueFromString(String values){
        String[] dataToSplit = values.split(":");
        String data = dataToSplit[1];
        data = data.substring(0, data.length()-1);
        String[] valueToParse = data.split(",");
        double[] jointData = new double[valueToParse.length];
        for (int i = 0; i < valueToParse.length; i++){
            String[] splittedValue = valueToParse[i].trim().split(" ");
            jointData[i] = Double.parseDouble(splittedValue[1]);
        }
        setAllValues(jointData);
    }

    private void setAllValues(double[] data) {
        j1 = data[0];
        j2 = data[1];
        j3 = data[2];
        j4 = data[3];
        j5 = data[4];
        j6 = data[5];
    }

    @Override
    public String toJsonString() throws JsonProcessingException {
        return this.toJson();
    }

    @Override
    public void setRandomValues() {
        Random rand = new Random();
        j1 = rand.nextDouble(0, 360);
        j2 = rand.nextDouble(0, 360);
        j3 = rand.nextDouble(0, 360);
        j4 = rand.nextDouble(0, 360);
        j5 = rand.nextDouble(0, 360);
        j6 = rand.nextDouble(0, 360);
    }
}