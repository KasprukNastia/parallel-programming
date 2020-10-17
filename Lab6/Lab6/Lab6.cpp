#include <iostream>
#include <ctime>
#include "omp.h"
#include "mpi.h"

int main(int argc, char** argv)
{
    srand(time(NULL));
    const int MATRIX_DIMENSION = 5;
    const int MASTER_RANK = 0;

    // rank - curent process sequence number
    // size - total processes count
    int rank, size, tag = 10;
    MPI_Init(&argc, &argv);
    MPI_Comm_size(MPI_COMM_WORLD, &size);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Status status;

    // Code that will be executed only by the master process
    if (rank == MASTER_RANK) {
        // Generating of random matrix inside master process
        int master_matrix[MATRIX_DIMENSION][MATRIX_DIMENSION];
        for (int i = 0; i < MATRIX_DIMENSION; i++) {
            for (int j = 0; j < MATRIX_DIMENSION; j++) {
                master_matrix[i][j] = i == j ? 0 : rand() % 100;
            }
        }

        // Sending copies of the matrix to the different processes
        for(int i = 0; i < MATRIX_DIMENSION; i++) {
            int matrix_copy[MATRIX_DIMENSION][MATRIX_DIMENSION];
            std::copy(&master_matrix[0][0], &master_matrix[0][0] + MATRIX_DIMENSION * MATRIX_DIMENSION, &matrix_copy[0][0]);
            MPI_Send(matrix_copy, MATRIX_DIMENSION * MATRIX_DIMENSION, MPI_INT, i + 1, tag, MPI_COMM_WORLD);
        }
        
        // Receiving the results with the diagonal values from these processes
        int diagonal_values[MATRIX_DIMENSION];
        for (int i = 0; i < MATRIX_DIMENSION; i++) {
            MPI_Recv(diagonal_values + i, 1, MPI_INT, i + 1, tag, MPI_COMM_WORLD, &status);
            // Writing the diagonal value on [i][i] position
            master_matrix[i][i] = diagonal_values[i];
        }

        // Printing random matrix
        for (int i = 0; i < MATRIX_DIMENSION; i++)
        {
            for (int j = 0; j < MATRIX_DIMENSION; j++)
            {
                std::cout << master_matrix[i][j] << "\t";
            }
            std::cout << std::endl;
        }
    }
    // Code that will be executed by all processes except master
    else {
        // Reseiving the copy of the matrix from the master process
        int received_matrix[MATRIX_DIMENSION][MATRIX_DIMENSION];
        MPI_Recv(received_matrix, MATRIX_DIMENSION * MATRIX_DIMENSION, MPI_INT, MASTER_RANK, tag, MPI_COMM_WORLD, &status);

        // Parallel calculation of the diagonal value on [rank - 1][rank - 1] position
        int diagonal_value = 0;
        /* 
           shared      - shared variables
           reduction   - sets a local variable (diagonal_value), as well as the operation that
                         will be performed on local variables when leaving the parallel region ("+")
           num_threads - number of the threads that will execute the region
        */
        #pragma omp parallel shared(received_matrix) reduction (+: diagonal_value) num_threads(MATRIX_DIMENSION)
        {
            #pragma omp for
            for (int i = 0; i < MATRIX_DIMENSION; i++) {
                if (i == rank - 1) continue;
                diagonal_value += (received_matrix[rank - 1][i] + received_matrix[i][rank - 1]);
            }
        }

        // Sending back to the master process calculated diagonal value on [rank - 1][rank - 1] position
        int result[1];
        result[0] = diagonal_value;
        MPI_Send(result, 1, MPI_INT, MASTER_RANK, tag, MPI_COMM_WORLD);
    }

    MPI_Finalize();
    return 0;
}