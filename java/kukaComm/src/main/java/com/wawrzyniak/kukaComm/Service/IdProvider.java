package com.wawrzyniak.kukaComm.Service;

import java.util.concurrent.atomic.AtomicInteger;

public class IdProvider {
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
    private static final AtomicInteger id = new AtomicInteger(0);
    public static int getId() {
=======
    private static AtomicInteger id = new AtomicInteger(0);
    public static int getId(){
>>>>>>> add testSocket and kukaComm
=======
    private static final AtomicInteger id = new AtomicInteger(0);
    public static int getId() {
>>>>>>> add swagger docs, fix some whitespace issues
        return id.getAndIncrement();
    }
}