package com.wawrzyniak.testsocket.Model.Records;

public record Vector3 (double x, double y, double z){
    public Vector3 add(Vector3 vector) {
         double x = this.x + vector.x();
         double y = this.y + vector.y();
         double z = this.z + vector.z();
         return new Vector3(x, y, z);
    }

    public Vector3 difference(Vector3 vector) {
        return new Vector3(vector.x() - this.x, vector.y() - this.y, vector.z() - this.z);
    }

    public Vector3 divide(int divider){
        return new Vector3(this.x / divider, this.y / divider,this.z / divider);
    }
}
