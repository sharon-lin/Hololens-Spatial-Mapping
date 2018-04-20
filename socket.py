host = socket.gethostbyname('IP_ADDRESS')
port = 80
s.bind((host, port))

print('Waiting for a connection...')
while (True):
    s.listen(1)
    c, addr = s.accept()
    print('Connection from: ' + str(addr))

    data_length = 0
    while True:
        data = c.recv(1024)
        if not data:
            break
        buffer += data

        if data_length == 0 and b':' in buffer:
            # retrieve data_length
            data_length, ignored, buffer = buffer.partition(b':')
            data_length = int(data_length.decode())

        if data_length == 0:
            continue

        if len(buffer) < data_length:
            print('message not long enough')
            continue
        message = buffer[:data_length]
        buffer = buffer[data_length:]
        data_length = 0

        # DO SOMETHING WITH MESSAGE

        # returned_message is the message I want to send back to client
        c.send(returned_message.encode())  # echo
        # close connection 
        c.close()
        break
# close socket
s.close()
