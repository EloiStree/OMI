import socket
import sys

def main():
    # Set the UDP ports for listening and responding
    listen_port = 5044
    response_port = 2506

    # Create a UDP socket for listening
    udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    # Bind the socket to the specified listening port
    try:
        udp_socket.bind(('', listen_port))
    except Exception as e:
        print("Error occurred:", e)
        sys.exit(1)

    print(f"Listening for UDP messages on port {listen_port}")

    try:
        while True:
            # Receive UDP data and the address of the sender
            data, address = udp_socket.recvfrom(1024)

            # Convert the received bytes to Unicode string
            received_message = data.decode('utf-16')

            # Print the received message
            print(f"Received: {received_message}")

            # Create a new UDP socket for sending the response
            response_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

            # Prepare the response message with "ping"
            response_message = "ping"

            # Encode the response message to bytes in Unicode format
            response_data = response_message.encode('utf-16')

            # Send the response back to the sender's address on the response port
            response_socket.sendto(response_data, (address[0], response_port))

            # Close the response socket
            response_socket.close()
    except KeyboardInterrupt:
        print("UDP listener stopped.")
    finally:
        udp_socket.close()

if __name__ == "__main__":
    main()
