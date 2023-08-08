package com.wawrzyniak.testsocket.Service;

import java.util.concurrent.atomic.AtomicInteger;

public class IdProvider {
    private static AtomicInteger id = new AtomicInteger(0);
    public static int getId(){
        return id.getAndIncrement();
    }
}