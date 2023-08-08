package com.wawrzyniak.kukaComm.Service;

import java.util.concurrent.atomic.AtomicInteger;

public class IdProvider {
<<<<<<< refs/remotes/origin/main
    private static final AtomicInteger id = new AtomicInteger(0);
    public static int getId() {
=======
    private static AtomicInteger id = new AtomicInteger(0);
    public static int getId(){
>>>>>>> add testSocket and kukaComm
        return id.getAndIncrement();
    }
}