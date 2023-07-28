import socket
import sys

def main():
    # Set the UDP port number you want to listen to
    port = 5042

    # Create a UDP socket
    udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    # Bind the socket to the specified port
    try:
        udp_socket.bind(('', port))
    except Exception as e:
        print("Error occurred:", e)
        sys.exit(1)

    print(f"Listening for UDP messages on port {port}")

    try:
        while True:
            # Receive UDP data and the address of the sender
            data, address = udp_socket.recvfrom(1024)

            # Convert the received bytes to Unicode string
            #received_message = data.decode('utf-8')
            received_message = data.decode('utf-16')

            # Print the received message
            print(f"Received: {received_message}")
    except KeyboardInterrupt:
        print("UDP listener stopped.")
    finally:
        udp_socket.close()

if __name__ == "__main__":
    main()